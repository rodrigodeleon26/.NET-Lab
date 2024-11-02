import { ArticulosService } from './../../services/articulos.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { fromEvent, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-articulos',
  templateUrl: './articulos.component.html',
  styleUrl: './articulos.component.css'
})
export class ArticulosComponent implements OnInit, OnDestroy {
  DatosArticuloForm: FormGroup;

  loading: boolean = false;

  articulos: any[] = [];
  articulosModal: any[] = [];
  busqueda: string = '';
  busquedaModal: string = '';
  isModalVisible: boolean = false;
  editando: any = null;

  private searchSubscription: Subscription | undefined;
  private searchModalSubscription: Subscription | undefined;

  constructor(
    private fb: FormBuilder,
    private articulosService: ArticulosService,
  ) {
    this.DatosArticuloForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(50)]],
    });
   }

  ngOnInit(): void {
    this.loading = true;
    
    const searchInput = document.getElementById('buscar');
    const searchInputModal = document.getElementById('buscarModal');
    if (searchInput) {
      this.searchSubscription = fromEvent(searchInput, 'input')
        .pipe(debounceTime(1000))
        .subscribe(() => {
          this.onSearch();
        });
    }

    this.articulosService.getArticulos().subscribe({
      next: (data) => {
        this.articulos = data;
        this.articulosModal = data;
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      },
      complete: () => {
        this.loading = false;
      }
    })
    
  }

  ngOnDestroy(): void {
    if (this.searchSubscription) {
      this.searchSubscription.unsubscribe();
    }
    if (this.searchModalSubscription) {
      this.searchModalSubscription.unsubscribe();
    }
  }

  onSearch(): void {
    if (!this.busqueda || this.busqueda === '') {
      this.articulosService.getArticulos().subscribe({
        next: (data) => {
          this.articulos = data;
        },
        error: (error) => {
          console.error(error);
        }
      });
      return;
    }
    this.articulosService.getArticulosFiltrados(this.busqueda).subscribe({
      next: (data) => {
        this.articulos = data;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  onSearchModal(): void {
    if (!this.busquedaModal || this.busquedaModal === '') {
      this.articulosService.getArticulos().subscribe({
        next: (data) => {
          this.articulosModal = data;
        },
        error: (error) => {
          console.error(error);
        }
      });
      return;
    }
    this.articulosService.getArticulosFiltrados(this.busquedaModal).subscribe({
      next: (data) => {
        this.articulosModal = data;
      },
      error: (error) => {
        console.error(error);
      }
    });

  }

  onModalContainerClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('fixed')) {
      this.hideModal();
    }
  }

  showModal(): void {
    this.isModalVisible = true;
    setTimeout(() => {
      const searchInputModal = document.getElementById('buscarModal');
      if (searchInputModal) {
        this.searchModalSubscription = fromEvent(searchInputModal, 'input')
          .pipe(debounceTime(1000))
          .subscribe(() => {
            this.onSearchModal();
          });
      }
    }, 0); // Asegura que el DOM se haya actualizado
  }

  hideModal(): void {
    this.isModalVisible = false;
    if (this.searchModalSubscription) {
      this.searchModalSubscription.unsubscribe();
    }
  }

  agregarArticulo(): void {
    console.log(this.DatosArticuloForm.value);
    this.articulosService.addArticulo(this.DatosArticuloForm.value).subscribe({
      next: (data) => {
        this.DatosArticuloForm.reset();
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
        this.onSearch();
        this.onSearchModal();
      }
    });
  }

  elegirParaEditar(articuloId: string){
    if (articuloId === '' || articuloId === null || articuloId == this.editando.id) {
      this.editando = null;
      this.DatosArticuloForm.reset();
      return;
    }
    this.editando = this.articulos.find(a => a.id === articuloId);
    this.DatosArticuloForm.patchValue(this.editando);
    console.log(this.editando);
  }

}
