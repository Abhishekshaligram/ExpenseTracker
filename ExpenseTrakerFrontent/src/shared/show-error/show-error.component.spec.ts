import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowErrorComponent } from './show-error.component';

describe('ShowErrorComponent', () => {
  let component: ShowErrorComponent;
  let fixture: ComponentFixture<ShowErrorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShowErrorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShowErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
