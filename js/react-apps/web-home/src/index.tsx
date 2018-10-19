import './Bootstrap/bootstrap-reboot.css';
import './Bootstrap/bootstrap-grid.css';
import './Bootstrap/bootstrap.css';

import configureStore from './Store';
import routes from './Routes';
import * as ReactDOM from 'react-dom';
import 'window.requestanimationframe';

configureStore();

ReactDOM.render(routes(), document.getElementById('root') as HTMLElement);
