import { createAction, props } from '@ngrx/store';

export const create = createAction(
  '[Project File] Create Project File',
  props<{name: string}>()
);

export const remove = createAction(
  '[Project File] Remove Project File',
  props<{id: string}>()
);

export const reset = createAction('[Project File] Reset');
