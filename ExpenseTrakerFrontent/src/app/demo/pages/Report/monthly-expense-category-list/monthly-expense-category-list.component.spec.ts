import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthlyExpenseCategoryListComponent } from './monthly-expense-category-list.component';

describe('MonthlyExpenseCategoryListComponent', () => {
  let component: MonthlyExpenseCategoryListComponent;
  let fixture: ComponentFixture<MonthlyExpenseCategoryListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MonthlyExpenseCategoryListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MonthlyExpenseCategoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
