# RhythmScrobbler

RhythmScrobbler is an application that allows you to connect your Last.fm account and automatically scrobble songs played in rhythm games like Clone Hero and YARG.

## Features

- **Last.FM Integration**: Connect your Last.FM account to scrobble songs played in supported games.
- **Game Path Selection**: Easily select the path of the game you want to scrobble from.
- **Real-time Scrobbling**: Automatically scrobble songs as you play them in the game.
- **Scrobbles History**: View logs of scrobbled songs and any errors.


## Images
![Home View of RhythmScrobbler](images/HomeView.png?raw=true "Home View of RhythmScrobbler")

![Scrobbles History of RhythmScrobbler](images/Scrobbles.png?raw=true "Scrobbles History of RhythmScrobbler")




## Usage

1. **Configure Last.fm**:
    - Either if you are building or using a release version, you need to configure it with your Last.FM API settings.
    - Edit the `appsettings.json` file in the root directory.
    - Add your Last.fm API key and secret:
        ```json
        {
          "LastFm": {
            "ApiKey": "your_api_key",
            "SharedSecret": "your_api_secret"
          }
        }
        ```

2. **Connect Last.fm**:
    - Open the application and connect your Last.fm account by clicking the **Login** button.

3. **Select Game Path**:
    - In the games list, select either Clone Hero or Yarg.
    - Click on the **folder** icon to select the path where the `currentsong.txt` file is located.
    - For Clone Hero, the path is usually `C:\Users\{YourUsername}\Documents\Clone Hero`.
    - For Yarg, the path is usually `%LOCALAPPDATA%\YARC\YARG\release`.
    - For Yarg, you can also go to **Settings** > **All Settings** > **File Management** > **Open Persistent Data Path** to locate the correct folder.

    **Note for Clone Hero**:
    1. Go to **Clone Hero Settings**.
    2. Navigate to **General**.
    3. Under **Streamer Settings**, turn on **EXPORT CURRENT SONG**.
    4. In `settings.ini`, change the `custom_song_export` to `%s%n%a%n%b`:
        ```ini
        custom_song_export = %s%n%a%n%b
        ```

    **Note for Yarg**:
    - Yarg already exports the current songs automatically.

4. **Play and Scrobble**:
    - Start playing songs in the game, and they will be automatically scrobbled to your Last.fm account.

5. **My Scrobbles**:
    - Check the logs to see the scrobbled songs and any errors.



## Building

1. **Clone the repository**:
    ```sh
    git clone https://github.com/yourusername/RhythmScrobbler.git
    cd RhythmScrobbler
    ```

2. **Install dependencies**:
    ```sh
    dotnet restore
    ```

3. **Configure Last.fm**:
    - Create a `appsettings.json` file in the root directory.
    - Add your Last.fm API key and secret:
        ```json
        {
          "LastFm": {
            "ApiKey": "your_api_key",
            "SharedSecret": "your_api_secret"
          }
        }
        ```

4. **Run the application**:
    ```sh
    dotnet run
    ```


## Technology Stack

- **.NET**: The application is built using .NET.
- **Avalonia UI**: The user interface is created with Avalonia UI.
- **MVVM with ReactiveUI**: Utilized for the Model-View-ViewModel architecture.
- **LiteDB**: Used to persist data.
- **Hqub.LastFM**: Library to interact with Last.fm.

## Contributing

Feel free to submit issues or pull requests. Contributions are welcome!

## License

This project is licensed under the MIT License.
