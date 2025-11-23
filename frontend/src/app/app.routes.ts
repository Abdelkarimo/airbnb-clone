import { Routes } from '@angular/router';
import { Home } from './features/home-page/home/home';
import { Listings } from './features/listings-page/listings/listings';
import { ListingsList } from './features/listings/list/listing-list';
import { ListingsCreateEdit } from './features/listings/create-edit/listings-create-edit';
import { listingExistsGuard } from './features/listings/services/listing-exists.guard';
import { ListingsDetail } from './features/listings/detail/listings-detail';


export const routes: Routes = [
  { path: 'home', component: Home },
  { path: '', pathMatch: 'full', redirectTo: 'home' },
  { path: 'properties', component: Listings },
  {
    path: 'host',
    children: [
      { path: '', component: ListingsList },
      { path: 'create', component: ListingsCreateEdit },
      { path: 'edit/:id', component: ListingsCreateEdit, canActivate: [listingExistsGuard] },
      { path: 'detail/:id', component: ListingsDetail, canActivate: [listingExistsGuard] },
    ],
  },
  { path: '**', redirectTo: 'home', pathMatch: 'full' }
];
