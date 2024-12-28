import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-data-protection',
  standalone: false,
  
  templateUrl: './data-protection.component.html',
  styleUrls: ['./data-protection.component.css']
})
export class DataProtectionComponent implements OnInit {

  ngOnInit(): void {
    window.scrollTo(0, 0);
  }

}
