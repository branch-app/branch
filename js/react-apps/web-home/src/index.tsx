import './Bootstrap/bootstrap-reboot.css';
import './Bootstrap/bootstrap-grid.css';
import './Bootstrap/bootstrap.css';

import configureStore from './Store';
import routes from './Routes';
import * as ReactDOM from 'react-dom';
import 'window.requestanimationframe';

const { store, history } = configureStore();

ReactDOM.render(routes(store, history), document.getElementById('root') as HTMLElement);
