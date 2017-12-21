import { Component, OnInit,AfterViewInit } from '@angular/core';
import { Location } from '@angular/common';
declare var $:any;

@Component({
  selector: 'app-error404',
  templateUrl: './error404.component.html',
  styleUrls: [
    './error404.component.css'
  ]
})
export class Error404Component implements OnInit,AfterViewInit {

  constructor(private _location: Location) { }
  ngOnInit() {
  }
  ngAfterViewInit() {
       /* Background Slider */
  $(function() {
    $('body').vegas({
      slides: [
        { src: 'assets/img/img1.jpg',fade:1000 },
        { src: 'assets/img/img2.jpg',fade:1000 },
        { src: 'assets/img/img3.jpg',fade:1000 },
    
      ]
    });
    });
    
    $(".timeset").countdowntimer({
                minutes : 0, /* Set minutes */
                seconds : 20, /* Set seconds */
                size : "lg",
                tickInterval : 1,
                timeSeparator : ":",
                timeUp : function() {
                  $(".timeset").html('<span class="opps">go <br />home!!</span>');
                  $('.timer img').addClass('animated infinite  flash');
                  $('.upper-mid img').addClass('animated infinite  shake');
                  $('.timeset span').addClass('animated infinite  bounce');
                  //$('img').addClass('animated infinite  shake');
                }
        });
  }
 /*  backClicked() {
    this._location.back();
  } */
}
