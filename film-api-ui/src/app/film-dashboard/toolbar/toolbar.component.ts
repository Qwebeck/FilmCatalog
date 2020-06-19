import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit {
  filmName: string;
  filmDirector: string;
  filmGenres: string[] = [
    "action", "comedy"
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
