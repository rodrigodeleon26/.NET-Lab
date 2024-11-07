import { TestBed } from '@angular/core/testing';

import { SegurosMedicosService } from './seguros-medicos.service';

describe('SegurosMedicosService', () => {
  let service: SegurosMedicosService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SegurosMedicosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
