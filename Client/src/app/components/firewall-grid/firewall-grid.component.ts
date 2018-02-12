import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FirewallService } from '../../services/firewall.service';
import { Firewall } from '../../interfaces/firewall';
import { DialogService } from 'ng2-bootstrap-modal/dist/dialog.service';
import { ConfirmComponent } from '../../components/confirm.component';


@Component({
  selector: 'app-firewall-grid',
  templateUrl: './firewall-grid.component.html',
  styleUrls: ['./firewall-grid.component.css']
})
export class FirewallGridComponent implements OnInit {

  constructor(public firewallService:FirewallService,
    private dialogService: DialogService) { }

  ngOnInit() {
  }

  delete (fw: Firewall) {
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Delete Firewall Setting',
      message: 'Are you sure you wish to permanently delete the firewall rule "' + fw.ruleName + '"?'
    }).subscribe((isConfirmed) => {
        // We get dialog result
        if (isConfirmed) {
          this.firewallService.deleteFirewall(fw);
        } else {

        }
      });
  }
}
