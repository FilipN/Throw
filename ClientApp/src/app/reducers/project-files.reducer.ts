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
  // FIXME: using Date.getTime to generate random and unique id of the project file.
  // generating Ids should be handled by server
  on(create, (state, { name }) =>
    [...state, {
      id: 'id' + (new Date()).getTime(),
      code: '',
      name: name,
      // set to current loged in usser
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
