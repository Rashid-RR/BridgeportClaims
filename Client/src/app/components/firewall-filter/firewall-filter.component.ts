import { Component, OnInit , AfterViewInit } from '@angular/core';
import { FirewallService } from '../../services/firewall.service';

declare var $: any;

@Component({
  selector: 'app-firewall-filter',
  templateUrl: './firewall-filter.component.html',
  styleUrls: ['./firewall-filter.component.css']
})
export class FirewallFilterComponent implements OnInit, AfterViewInit {

  constructor(public firewallService: FirewallService) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    $('[inputs-mask]').inputmask();
    $('[data-mask]').inputmask();
  }
  clear() {
    this.firewallService.form.reset();
  } 

}
