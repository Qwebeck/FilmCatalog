import { Component, OnInit } from '@angular/core';
import { Film } from '../../film';
import { ActivatedRoute } from '@angular/router';
import { FilmService } from '../../film.service';

@Component({
  selector: 'app-editor-view',
  templateUrl: './editor-view.component.html',
  styleUrls: ['./editor-view.component.scss']
})
export class EditorViewComponent implements OnInit {

  film: Film = { title: '', addedBy: '', description: '', genre: '', filmID: 0};
  showPreview: boolean = true;
  editing: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private filmService: FilmService
  ) { }

  ngOnInit(): void {
    let id = +this.route.snapshot.paramMap.get("id");
    if (id) {
      this.filmService.getFilm(id)
        .subscribe( f => {
          this.editing = true;
          // creating copy of object, so edited film will not reference to existing film
          this.film ={ ...f};
        });

    }
  }
}
