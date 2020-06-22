import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-comment-editing-block',
  templateUrl: './comment-editing-block.component.html',
  styleUrls: ['./comment-editing-block.component.scss']
})
export class CommentEditingBlockComponent implements OnInit {

  content: string;

  constructor() { }

  ngOnInit(): void {
  }
  
  publish(): void {

  }
}
