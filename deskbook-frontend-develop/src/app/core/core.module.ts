import { NgModule } from '@angular/core';
import { HeaderComponent } from './components/header/header.component';
import { LandingPageComponent } from './components/landing-page/landing-page.component';
import { MasterComponent } from './components/master/master.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { RouterModule } from '@angular/router';
import { AuthGuard } from './services/guard/auth.guard';
import { AuthService } from './services/auth/auth.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthInterceptor } from './services/interceptor/auth.interceptor';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { AdminHeaderComponent } from './components/admin-header/admin-header.component';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from './components/loader/loader.component';

@NgModule({
  declarations: [
    HeaderComponent,
    LandingPageComponent,
    MasterComponent,
    PageNotFoundComponent,
    SidebarComponent,
    AdminHeaderComponent,
    MasterComponent,
    LoaderComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    ToastrModule.forRoot(),
    BrowserAnimationsModule,
    NgSelectModule,
    NgbPopoverModule,
    HttpClientModule,
    BrowserModule,
  ],
  providers: [
    AuthGuard,
    AuthService,
    //  AuthInterceptor
    {
      provide: HTTP_INTERCEPTORS,
      multi: true,
      useClass: AuthInterceptor,
    },
  ],
  exports: [
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    ToastrModule,
    BrowserAnimationsModule,
    LoaderComponent,
  ],
})
export class CoreModule {}
