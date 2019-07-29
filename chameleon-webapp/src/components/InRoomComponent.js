import React from 'react';

class InRoomComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      numberOfPeopleInRoom: null,
      numberOfChameleonsInRoom: null,
    };

    this.poll = this.poll.bind(this);

    this.poll();
  }

  poll() {
    fetch(process.env.REACT_APP_CHAMELEON_BACKEND_BASE_URL + '/api/room-status/?RoomCode=' + this.props.roomCode, {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'X-Chameleon-PersonId': this.props.personId,
      }
    })
      .then(response => response.json())
      .then(jsonBody => {
        this.setState({
          numberOfPeopleInRoom: jsonBody.PeopleCount,
          numberOfChameleonsInRoom: jsonBody.ChameleonCount,
        });
        if (jsonBody.TimeToPollMillisecond) {
          setTimeout(this.poll, jsonBody.TimeToPollMillisecond);
        }
      })
      .catch((error) => { console.error(error); })
  }
  
  render() {
    return (
      <div>
        Welcome {this.props.personId} to room {this.props.roomCode}.
        <br />
        {this.state.numberOfPeopleInRoom == null ? (
          <span>Loading...</span>
        ) : (
          <span>There are {this.state.numberOfPeopleInRoom} people in the room, {this.state.numberOfChameleonsInRoom} of them are Chameleons!</span>
        )}
      </div>
    );
  }
}

export default InRoomComponent;
