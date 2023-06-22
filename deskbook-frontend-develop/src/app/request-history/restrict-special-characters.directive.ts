import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[deskBookRestrictSpecialCharacters]',
})
export class RestrictSpecialCharactersDirective {
  getPastedText: any;
  key: any;
  inputValue: any;

  constructor(private el: ElementRef) {}

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    this.inputValue = this.el.nativeElement.value;
    this.key = event.key;
    if (/[^a-zA-Z' ]/.test(this.key)) {
      event.preventDefault();
    }
  }

  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent) {
    const pastedText = this.getPastedText(event);
    if (/[^a-zA-Z' ]/.test(pastedText)) {
      event.preventDefault();
    }
  }
}
