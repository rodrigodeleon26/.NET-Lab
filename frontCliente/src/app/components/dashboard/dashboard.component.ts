import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit{
  constructor(
    private router: Router,
    private authService: AuthService,
    private userService: UserService
  ) {}

  fullName: string = '';

  ngOnInit(): void {
    this.userService.getDatosPersonales().subscribe({
      next: (res:any) => this.fullName = res.fullName,
      error: (err:any) => console.log("Error al obtener los datos personales:\n", err)

    });
  }

  onLogout() {
    this.authService.deleteToken(); 
    this.router.navigateByUrl('/login');
  }
}
