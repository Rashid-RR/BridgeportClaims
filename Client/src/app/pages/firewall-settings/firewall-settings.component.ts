import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { FirewallService } from '../../services/firewall.service';

@Component({
  selector: 'app-firewall-settings',
  templateUrl: './firewall-settings.component.html',
  styleUrls: ['./firewall-settings.component.css']
})
export class FirewallSettingsComponent implements OnInit {

  expanded= true;
  constructor(public firewallService: FirewallService) { }

  ngOnInit() {
  }



}
