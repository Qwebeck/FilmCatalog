import { Component, OnInit } from '@angular/core';
import { Film } from '../../film';

@Component({
  selector: 'app-editor-view',
  templateUrl: './editor-view.component.html',
  styleUrls: ['./editor-view.component.scss']
})
export class EditorViewComponent implements OnInit {

  film: Film = { title: '', addedBy: '', description: '', genre: '', filmID: 0};
  showPreview: boolean = true;

  constructor() { }

  ngOnInit(): void {
  }
}
