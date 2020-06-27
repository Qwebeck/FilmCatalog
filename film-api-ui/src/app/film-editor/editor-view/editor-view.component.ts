import { Component, OnInit } from '@angular/core';
import { Film } from '../../interfaces/film';
import { ActivatedRoute } from '@angular/router';
import { FilmService } from '../../services/film.service';

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
          this.film ={ ...f};
          this.filmService.assignImage(this.film)
            .subscribe(f => this.film = {...f});
        });

    }
  }
}
