import { Component, OnInit } from '@angular/core';

// Services
import { UnpaidScriptService } from "../../services/unpaid-script.service";

@Component({
  selector: 'app-unpaid-script',
  templateUrl: './unpaid-script.component.html',
  styleUrls: ['./unpaid-script.component.css']
})
export class UnpaidScriptComponent implements OnInit {

  constructor(
    private us: UnpaidScriptService
  ) { }

  ngOnInit() {
    this.us.search();
  }

}
