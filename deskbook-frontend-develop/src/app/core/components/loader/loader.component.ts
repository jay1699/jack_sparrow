import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { LoaderService } from './loader.service';

@Component({
  selector: 'desk-book-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss']
})
export class LoaderComponent implements OnInit {

  constructor(private loaderService: LoaderService, private cdRef: ChangeDetectorRef) { }
  showLoader = false;
  ngOnInit(): void {
    this.init()
  }
  init() {
    this.loaderService.getloaderObserver().subscribe((status) => {
      this.showLoader = (status === 'start');
      this.cdRef.detectChanges();
    });
  }

}
