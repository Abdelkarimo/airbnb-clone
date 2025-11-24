import { Routes } from '@angular/router';
import { Home } from './features/home-page/home/home';
import { Listings } from './features/listings-page/listings/listings';
import { ListingsList } from './features/listings/list/listing-list';
import { ListingsCreateEdit } from './features/listings/create-edit/listings-create-edit';
import { listingExistsGuard } from './features/listings/services/listing-exists.guard';
import { AuthGuard } from './core/guards/auth.guard';
import { ListingsDetail } from './features/listings/detail/listings-detail';
import { Login } from './features/auth/login';
import { Register } from './features/auth/register';
import { Dashboard } from './features/admin/dashboard';
import { BookingComponent } from './features/booking/booking';
import { PaymentComponent } from './features/payment/payment';
import { ChatWindow } from './features/message/chat-window';


export const routes: Routes = [
  { path: 'home', component: Home },
  { path: '', pathMatch: 'full', redirectTo: 'home' },
  { path: 'properties', component: Listings },
  { path: 'auth/login', component: Login },
  { path: 'auth/register', component: Register },
  {
    path: 'host',
    children: [
      { path: '', component: ListingsList },
      { path: 'create', component: ListingsCreateEdit, canActivate: [AuthGuard] },
      { path: 'edit/:id', component: ListingsCreateEdit, canActivate: [listingExistsGuard] },
      { path: 'detail/:id', component: ListingsDetail, canActivate: [listingExistsGuard] },
    ],
  },
  { path: 'admin', component: Dashboard, canActivate: [AuthGuard] },
  { path: 'booking', component: BookingComponent, canActivate: [AuthGuard] },
  { path: 'payment/:id', component: PaymentComponent, canActivate: [AuthGuard] },
  { path: 'messages', component: ChatWindow, canActivate: [AuthGuard] },
  { path: '**', redirectTo: 'home', pathMatch: 'full' }
];
