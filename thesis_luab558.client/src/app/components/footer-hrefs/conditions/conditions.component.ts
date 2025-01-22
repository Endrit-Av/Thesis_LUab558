import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-conditions',
  standalone: false,
  
  templateUrl: './conditions.component.html',
  styleUrls: ['./conditions.component.css']
})
export class ConditionsComponent implements OnInit {

  ngOnInit(): void {
    window.scrollTo(0, 0);
  }

}
