import * as React from 'react';
import { Link, Route, Switch } from 'react-router-dom';

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap';

import './App.css';

import IUserAction from "./../models/IUserAction";

import ViewHome from './ViewHome/ViewHome';
import ViewCustomers from './ViewCustomers/ViewCustomers';
import ViewAbout from './ViewAbout/ViewAbout';


import Banner from "./Banner";
import TopNav from "./Topnav";

interface AppProperties {
    appTitle?: string;
}


interface AppState {

}

export default class App extends React.Component<AppProperties, AppState> {

    private actionsTopNav: IUserAction[];
    private actionsCustomersView: IUserAction[];

    constructor(props: AppProperties) {
        super(props);
        this.actionsTopNav = [
            { caption: "Home", actionFunction: () => { } },
            { caption: "Customers", actionFunction: () => { } },
            { caption: "About", actionFunction: () => { } }

        ];
    }

    public static defaultProps: Partial<AppProperties> = {
        appTitle: "CPT React Demo App"
    };

    render() {
        return (
            <div id="page-container" className="container">

                <Banner appTitle={this.props.appTitle!} >
                    <TopNav userActions={this.actionsTopNav} />
                </Banner>

                <Switch>
                    <Route path="/" exact component={ViewHome} />
                    <Route path="/customers" component={ViewCustomers} />
                    <Route path="/about" component={ViewAbout} />
                </Switch>

            </div>
        );
    }

    componentDidMount() {
        console.log("App.componentDidMount");
    }

    componentWillMount() {
        console.log("App.componentWillMount");
    }

    componentWillUpdate() {
        console.log("App.componentWillUpdate");
    }
}