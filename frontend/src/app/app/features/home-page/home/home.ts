import { Component, HostListener, ElementRef, AfterViewInit } from '@angular/core';
import { HeroCard } from "../hero-card/hero-card";
import { ListingCard } from "../listing-card/listing-card";
import { StackedCards } from "../stacked-cards/stacked-cards";

@Component({
  selector: 'app-home',
  imports: [HeroCard, ListingCard, StackedCards],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {

}
