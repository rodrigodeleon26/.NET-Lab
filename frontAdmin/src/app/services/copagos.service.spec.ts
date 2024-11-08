import { TestBed } from '@angular/core/testing';

import { CopagosService } from './copagos.service';

describe('CopagosService', () => {
  let service: CopagosService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CopagosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
