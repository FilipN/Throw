import { create, remove, reset } from '../actions/project-files.actions';
import { ProjectFile } from '../models/project-file.model';
import { on, createReducer } from '@ngrx/store';

export const initialState: ProjectFile[] = [
  {
    id: 'uuid-neki-dugacki-jedinstveni',
    code: '',
    name: 'Prvi projekat',
    author: 'id_autora_neki_1',
    created: new Date(),
    lastUpdated: new Date(),
  },
  {
    id: 'isto-uuid-neki-dugacki-jedinstveniji-od-prvog',
    code: '',
    name: 'Nije prvi projekat',
    author: 'id_autora_neki_1',
    created: new Date(),
    lastUpdated: new Date(),
  }
];

const _projectFilesReducer = createReducer(initialState,

  on(create, (state, { name, id }) =>
    [...state, {
      id: id,
      code: '',
      name: name,
      // FIXME: set to current loged in usser
      author: 'id_autora_neki_1',
      created: new Date(),
      lastUpdated: new Date(),
    }]
  ),
  on(remove, (state, { id }) => {
    const newState = [...state];
    const index = newState.findIndex(file => file.id === id);
    newState.splice(index, 1);
    return newState;
  }),
  on(reset, state => initialState),
);

export function projectFilesReducer(state, action) {
  return _projectFilesReducer(state, action);
}
