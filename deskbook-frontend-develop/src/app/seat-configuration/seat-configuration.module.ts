import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SeatConfigurationRoutingModule } from './seat-configuration-routing.module';
import { SeatConfigurationComponent } from './seat-configuration/seat-configuration.component';
import { SeatConfigurationHeaderComponent } from './seat-configuration-header/seat-configuration-header.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminFooterComponent } from './admin-footer/admin-footer.component';
import { FilterSeatPipe } from './filter-seat.pipe';
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [
    SeatConfigurationHeaderComponent,
    SeatConfigurationComponent,
    AdminFooterComponent,
    FilterSeatPipe,
  ],
  imports: [
    CommonModule,
    SeatConfigurationRoutingModule,
    ReactiveFormsModule,
    NgSelectModule,
    FormsModule,
    NgbPopoverModule,
  ],
})
export class SeatConfigurationModule { }
