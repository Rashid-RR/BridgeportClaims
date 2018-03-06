import { Component, NgZone, OnInit, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements AfterViewInit {

  constructor(private zone: NgZone) { }

  ngAfterViewInit() {

    this.zone.runOutsideAngular(() => {
      
    })
  }

}
