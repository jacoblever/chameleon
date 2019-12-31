import React from 'react';
import JoinRoomComponent from './JoinRoomComponent';
import InRoomComponent from './InRoomComponent';
import Cookies from 'js-cookie';
import Config from '../Config'
import logo from './logo.jpg'
import './ChameleonComponent.css';

class ChameleonComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      roomCode: Cookies.get('roomCode'),
      personId: Cookies.get('personId')
    };
    this.warmUpBackend();

    this.onRoomJoined = this.onRoomJoined.bind(this);
    this.onRoomLeft = this.onRoomLeft.bind(this);
  }

  warmUpBackend() {
    fetch(Config.backendBaseApiUrl() + 'warm-up', {
      method: 'GET',
    })
  }

  onRoomJoined(roomCode, personId) {
    Cookies.set('personId', personId, { expires: 1 });
    Cookies.set('roomCode', roomCode, { expires: 1 });
    this.setState({roomCode: roomCode, personId: personId});
  }

  onRoomLeft() {
    Cookies.remove('personId');
    Cookies.remove('roomCode');
    this.setState({roomCode: null, personId: null});
  }

  gameComponent() {
    if (this.state.roomCode != null) {
      return <InRoomComponent
        roomCode={this.state.roomCode}
        personId={this.state.personId}
        onRoomLeft={this.onRoomLeft}
      />
    } else {
      return <JoinRoomComponent
        onRoomJoined={this.onRoomJoined}
      />
    }
  }
  
  render() {
    return (
      <div className="Chameleon">
        <header className="Chameleon-header">
          <p>
            Welcome to Chameleon!
          </p>
          <img
            src={logo}
            className="Chameleon-logo"
            alt="Chameleon!"
          />
        </header>
        <div className="Chameleon-game">
          {this.gameComponent()}
        </div>
      </div>
    );
  }
}

export default ChameleonComponent;
