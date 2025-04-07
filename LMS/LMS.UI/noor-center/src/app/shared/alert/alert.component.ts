import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})
export class AlertComponent implements OnInit {
  @Input() message: string = 'Something went wrong!';
  @Output() closed = new EventEmitter<void>();

  ngOnInit(): void {
    setTimeout(() => {
      this.closeAlert();
    }, 6000);
  }

  closeAlert() {
    this.closed.emit();
  }
}
