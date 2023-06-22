import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookSeatComponent } from './book-seat/book-seat.component';

const routes: Routes = [{ path: '', component: BookSeatComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BookSeatRoutingModule {}
