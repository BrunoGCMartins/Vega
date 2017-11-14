import { AdminComponent } from './components/admin/admin.component';

import { BrowserXhrWithProgress, ProgressService } from './services/progress.service';
import { BrowserXhr } from '@angular/http';
import { PhotoService } from './services/photo.service';
import { FormsModule } from '@angular/forms'; 
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UniversalModule } from 'angular2-universal';
import { BrowserModule } from '@angular/platform-browser';
import { ChartModule } from 'angular2-chartjs';

import { VehicleService } from './services/vehicle.service';
import { AppComponent } from './components/app/app.component'
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { VehicleFormComponent } from './components/vehicle-form/vehicle-form.component';
import { VehicleListComponent } from "./components/vehicle-list/vehicle-list.component";
import { PaginationComponent } from "./components/shared/pagination.component";
import { ViewVehicleComponent } from "./components/view-vehicle/view-vehicle.component";
import { Auth } from "./services/auth.service";
import { AuthGuard } from "./services/auth-gaurd.service";
import { AUTH_PROVIDERS } from "angular2-jwt/angular2-jwt";
import { AdminAuthGuard } from "./services/admin-auth-guard.service";





@NgModule({
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,
        VehicleFormComponent,
        VehicleListComponent,
        PaginationComponent,
        ViewVehicleComponent,
        AdminComponent
    ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        FormsModule,
        BrowserModule,
        ChartModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'vehicles', pathMatch: 'full' },
            { path: 'vehicles', component: VehicleListComponent },
            { path: 'vehicles/new', component: VehicleFormComponent, canActivate: [ AdminAuthGuard ] },
            { path: 'vehicles/edit/:id', component: VehicleFormComponent, canActivate: [ AdminAuthGuard ] },
            { path: 'vehicles/:id', component: ViewVehicleComponent },
            { path: 'admin', component: AdminComponent}, //vai ser mostrada apenas se o user tiver o Admin Role
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
      Auth,
      AuthGuard,
      AUTH_PROVIDERS,
      AdminAuthGuard,
      AuthGuard,
      VehicleService,
      PhotoService,
      ProgressService,     
    ]
})
export class AppModule {
}
