import { Component, OnInit, ChangeDetectorRef, signal, computed } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ListingOverviewVM } from '../../../core/models/listing.model';
import { ListingService } from '../../../core/services/listings/listing.service';
import { ListingCard } from '../listing-card/listing-card';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-listings',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, ListingCard, ReactiveFormsModule],
  templateUrl: './listings.html',
  styleUrls: ['./listings.css']
})
export class Listings implements OnInit {
  // raw data
  listings = signal<ListingOverviewVM[]>([]);
  loading = signal<boolean>(false);
  error = signal<string>('');

  // filters (signals)
  search = signal<string>('');
  destination = signal<string>('');
  type = signal<string>('');
  maxPrice = signal<number | null>(null);
  minRating = signal<number | null>(null);

  // amenities (form)
  form: FormGroup;

  constructor(
    private listingService: ListingService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      amenities: [[]]
    });
  }

  // list of amenities
  amenitiesList = [
    'Wi-Fi', 'Pool', 'AC', 'Kitchen', 'Washer', 'Dryer', 'TV', 'Heating',
    'Parking', 'Fireplace', 'Gym', 'Breakfast', 'Pets Allowed', 'Hot Tub', 'Elevator'
  ];

  toggleAmenity(amenity: string): void {
    const currentValue = this.form.get('amenities')?.value || [];
    const amenities = Array.isArray(currentValue) ? [...currentValue] : [];
    const index = amenities.indexOf(amenity);

    if (index > -1) amenities.splice(index, 1);
    else amenities.push(amenity);

    this.form.patchValue({ amenities });
  }

  isAmenitySelected(amenity: string): boolean {
    const amenities: string[] = this.form.get('amenities')?.value || [];
    return amenities.includes(amenity);
  }

  // arabic/english normalization
  private normalize(input?: string): string {
    if (!input) return '';
    let s = String(input).trim().toLowerCase();

    s = s.replace(/[\u0610-\u061A\u064B-\u065F\u06D6-\u06ED]/g, '');
    s = s.replace(/أ|إ|آ/g, 'ا');
    s = s.replace(/ة/g, 'ه');
    s = s.replace(/ى/g, 'ي');
    s = s.replace(/ؤ/g, 'و');
    s = s.replace(/ئ/g, 'ي');
    s = s.replace(/ک/g, 'ك').replace(/ی/g, 'ي');
    s = s.replace(/[^0-9a-z\u0600-\u06FF\s]/g, '');
    s = s.replace(/\s+/g, ' ').trim();

    return s;
  }

  // computed list of unique destinations - improved sorting
  destinations = computed<string[]>(() => {
    const allDestinations = this.listings()
      .map(l => l.destination || l.location) // Fallback to location if destination doesn't exist
      .filter((dest): dest is string => !!dest);

    return [...new Set(allDestinations)].sort((a, b) => a.localeCompare(b));
  });

  // computed list of unique types - improved sorting
  types = computed<string[]>(() => {
    const allTypes = this.listings()
      .map(l => l.type)
      .filter((type): type is string => !!type);

    return [...new Set(allTypes)].sort((a, b) => a.localeCompare(b));
  });

  // computed filtered list - improved filtering logic
  filtered = computed<ListingOverviewVM[]>(() => {
    const data = this.listings();
    if (!data || !Array.isArray(data) || data.length === 0) return [];

    const rawQuery = this.search().trim();
    const rawDest = this.destination().trim();
    const rawType = this.type().trim();
    const maxP = this.maxPrice();
    const minR = this.minRating();
    const selectedAmenities: string[] = this.form.get('amenities')?.value || [];

    const q = this.normalize(rawQuery);
    const destNormalized = this.normalize(rawDest);
    const typeNormalized = this.normalize(rawType);

    return data.filter(l => {
      const title = this.normalize(l.title);
      const destinationVal = this.normalize(l.destination || l.location); // Use destination with fallback to location
      const typeVal = this.normalize(l.type ?? '');
      const description = this.normalize(l.description ?? '');

      // Search matching
      const matchesSearch =
        !q ||
        title.includes(q) ||
        destinationVal.includes(q) ||
        description.includes(q);

      // Destination matching
      const matchesDestination =
        !destNormalized || destinationVal.includes(destNormalized);

      // Type matching
      const matchesType =
        !typeNormalized || typeVal.includes(typeNormalized);

      // Price matching
      const priceOk =
        maxP === null || l.pricePerNight <= maxP;

      // Rating matching
      const ratingOk =
        minR === null || (l.averageRating ?? 0) >= minR;

      // Amenities matching
      // const matchesAmenities = selectedAmenities.length === 0 || 
      //   selectedAmenities.every(amenity => 
      //     l.amenities?.includes(amenity) || false
      //   );

      return matchesSearch && matchesDestination && matchesType && priceOk && ratingOk; // && matchesAmenities;
    });
  });

  ngOnInit(): void {
    this.loadListings();

    // refresh on navigation
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd && this.router.url.startsWith('/listings')) {
        this.loadListings();
      }
    });
  }

  loadListings(): void {
    this.loading.set(true);
    this.error.set('');

    this.listingService.getPaged().subscribe({
      next: (res) => {
        this.listings.set(res.data || []);
        this.loading.set(false);
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Error loading listings:', err);
        this.error.set('Failed to load listings');
        this.loading.set(false);
        this.cdr.markForCheck();
      }
    });
  }

  resetFilters() {
    this.search.set('');
    this.destination.set('');
    this.type.set('');
    this.maxPrice.set(null);
    this.minRating.set(null);
    this.form.patchValue({ amenities: [] });
  }

  onDelete(id: number) {
    if (!confirm('Delete this listing?')) return;

    this.listingService.delete(id).subscribe({
      next: () => {
        this.listings.update(list => list.filter(l => l.id !== id));
      },
      error: () => {
        this.error.set('Failed to delete listing');
      }
    });
  }

  trackById(index: number, item: ListingOverviewVM): number {
    return item.id ?? index;
  }
}