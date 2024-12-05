import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent implements OnInit, OnDestroy {
  fullName: string | null = null;
  private routerSubscription: Subscription | null = null;

  constructor(
    private router: Router,
    private authService: AuthService,
    private toastr: ToastrService
  ) {}
  
  ngOnInit(): void {
    this.routerSubscription = this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        const claims = this.authService.getClaims();
        this.fullName = claims ? claims.fullName : null;
      }
    });
  }

  ngOnDestroy(): void {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }

  irInicio(): void {
    this.router.navigate(['inicio']);
  }

  logout(): void {
    this.authService.logout().subscribe(
      (response) => {
        this.authService.deleteToken();
        this.fullName = null;
        this.router.navigate(['login']);
      },
      (error) => {
        this.toastr.error('Error al cerrar sesión');
      }
    );
  }
}
