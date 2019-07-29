import React from 'react';

class InRoomComponent extends React.Component {
  render() {
    return (
      <div>
        Welcome {this.props.personId} to room {this.props.roomCode}
      </div>
    );
  }
}

export default InRoomComponent;
