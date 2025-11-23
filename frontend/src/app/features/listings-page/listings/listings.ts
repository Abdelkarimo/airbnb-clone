import { Component } from '@angular/core';
import { ListingCard } from '../listings-card/listing-card';

@Component({
  selector: 'app-listings',
  imports: [ListingCard],
  templateUrl: './listings.html',
  styleUrl: './listings.css',
})
export class Listings {

}
