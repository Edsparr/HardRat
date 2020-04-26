 import React, { Component } from 'react';

export class FetchData extends Component {
  displayName = FetchData.name

  constructor(props) {
    super(props);
    this.state = { clients: [], loading: true };
    this.handleChange = this.handleChange.bind(this)

    fetch("Api/ServerHub/Clients")
      .then(c => c.json())
      .then(c => {
        this.setState({
          clients: c,
          isLoading: false
        })
      })

  }
  handleChange(event) {
    this.setState({ codeInput: event.target.value });
  }


  renderClients(clients) {
    return 
    <div>
      {clients.map(c => <p>{c.connectionId}:::{c.ip}</p> )}
    </div>
  }

  render() {
    let contents = this.state.isLoading
      ? <p><em>Loading...</em></p>
      : this.renderClients(this.state.clients);

    return (
      <div>
        <h1>Send code to client machine!</h1>
        <p>This component demonstrates fetching data from the server.</p>
        <textarea id="w3mission" rows="20" cols="50" value={this.state.codeInput} onChange={this.handleChange}>

        </textarea>
        {contents}


        <div>

        <button onClick={(event) => {
          fetch('api/ClientAction/Execute', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
              code: encodeURI(this.state.codeInput)
            })
          })
          }}>Execute code on clients</button>

          <button onClick={(event) => {
            fetch('api/ClientAction/Action/ShowTaskbar', {
              method: 'POST',
              headers: { 'Content-Type': 'application/json' },
            })
          }}>Show taskbar</button>

          <button onClick={(event) => {
            fetch('api/ClientAction/Action/HideTaskbar', {
              method: 'POST',
              headers: { 'Content-Type': 'application/json' },
            })
          }}>Hide taskbar</button>

          
        </div>

      </div>
    );
  }
}
