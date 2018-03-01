import { Component, NgZone, OnInit, AfterViewInit } from '@angular/core';

declare var Word: any;
declare var Office: any;
declare var OfficeExtension: any;
@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements AfterViewInit {

  constructor(private zone: NgZone) { }

  ngAfterViewInit() {

    this.zone.runOutsideAngular(() => {
      // Run a batch operation against the Word object model.
      Word.run(function (context) {

        // Create a proxy object for the document.
        var thisDocument = context.document;
        console.log(thisDocument);
        // Queue a command to load content control properties.
        context.load(thisDocument, 'contentControls/id, contentControls/text, contentControls/tag');

        // Synchronize the document state by executing the queued commands, 
        // and return a promise to indicate task completion.
        return context.sync().then(function () {
          if (thisDocument.contentControls.items.length !== 0) {
            for (var i = 0; i < thisDocument.contentControls.items.length; i++) {
              console.log(thisDocument.contentControls.items[i].id);
              console.log(thisDocument.contentControls.items[i].text);
              console.log(thisDocument.contentControls.items[i].tag);
            }
          } else {
            console.log('No content controls in this document.');
          }
        });
      })
        .catch(function (error) {
          console.log('Error: ' + JSON.stringify(error));
          if (error instanceof OfficeExtension.Error) {
            console.log('Debug info: ' + JSON.stringify(error.debugInfo));
          }
        });
    })
  }

}
