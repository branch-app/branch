import configureStore from './Store';
import routes from './Routes';
import * as ReactDOM from 'react-dom';

import 'bootstrap/dist/css/bootstrap.min.css';

configureStore();

ReactDOM.render(routes(), document.getElementById('root') as HTMLElement);
