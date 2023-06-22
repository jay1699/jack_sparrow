import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'desk-book-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  addButtonVisible: boolean = false;
  @Output() showAddButtonEvent = new EventEmitter();
  constructor() { }

  ngOnInit(): void {
  }
  onClickManage(value: any) {
    this.addButtonVisible = true;
    this.showAddButtonEvent.emit(this.addButtonVisible)
  }


}
