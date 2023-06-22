import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filterBookseat',
})
export class FilterBookseatPipe implements PipeTransform {
  transform(array: any[], column: string): any[] {
    if (!array || !array.length || !column) {
      return [];
    }

    return array.filter((item) => item.column.name === column);
  }
}
