import { Component, OnInit, Input } from '@angular/core';
import { Film } from '../../interfaces/film';
import { AuthenticationService } from '../../menu/authentication.service';


@Component({
  selector: 'app-film-description',
  templateUrl: './film-description.component.html',
  styleUrls: ['./film-description.component.scss']
})
export class FilmDescriptionComponent implements OnInit {

  @Input() 
  film?: Film;
  
  @Input()
  readMode: boolean; 

  couldEdit: boolean = false;

  constructor(
    public auth: AuthenticationService
  ) { }

  ngOnInit(): void { 
    if ( !this.film ) {
      this.film = { title: '', filmID: 0, addedBy:'', genre: '', description: ''};
    }
    else {
      this.couldEdit = this.readMode && this.auth.currentUser && (this.auth.currentUser.name == this.film.addedBy || this.auth.currentUser.groups.includes("Administrators"))
    }
  }
}
