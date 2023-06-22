import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filterSeat',
})
export class FilterSeatPipe implements PipeTransform {
  transform(array: any[], columnName: string): any[] {
    if (!array || !array.length || !columnName) {
      return [];
    }

    return array.filter((item) => item.columnName === columnName);
  }
}
