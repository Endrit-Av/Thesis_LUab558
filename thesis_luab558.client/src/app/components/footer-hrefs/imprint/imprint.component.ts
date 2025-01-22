import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-imprint',
  standalone: false,

  templateUrl: './imprint.component.html',
  styleUrls: ['./imprint.component.css']
})
export class ImprintComponent implements OnInit {

  ngOnInit(): void {
    window.scrollTo(0, 0);
  }

}
