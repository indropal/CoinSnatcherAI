<h1>CoinSnatcherAI</h1>

<p align="center">
<img src="https://github.com/indropal/CoinSnatcherAI/blob/main/Images/Coin_Snatcher.png?raw=True" width="500" height="350"/>
</p>
<hr style = "border: 2px gray double"></hr>
<br>
This project presents a <b>micro-game</b> which is based on the idea of <b><em>Gamified Reinforcement Learning</em></b>. The Player has to compete against an <b>AI(RL) Agent</b> to collect <em>as many coins as possible</em> within a constrained amount of time. The project has been built with <b>Unity's <a href = "https://github.com/Unity-Technologies/ml-agents">ml-agents</a></b> package.
<br></br>

<b>The Game is available <u><a href = "https://indropal.github.io/CoinSnatcherAI/">HERE</a></u> for playing!<br/>
Try and beat the AI Agent! 😄

NOTE:
1. You'll need a keyboard to play the game.
2. Please give approximately 30 seconds for the game to load on your browser [Recommended: Google Chrome - other browsers should work fine].
3. Enjoy! 😃
</b>
<br>

<u><h3 id="player-controls">Player Controls:</h3></u>

In order to collect coins, the player needs to navigate / control the player-avatar to the coin. Upon successful collection, the collected coin will disappear from the game & update the <em><b>Player Score</b></em>.

<em>Movement Controls</em>:
<br>The following are the key mappings —
* ↑ / W &nbsp; : move Forward
* ↓ / S &nbsp;&nbsp; : move Backward
* ← / A &nbsp;&nbsp;: turn Left
* → / D &nbsp;&nbsp;: turn Right
* ⎵ (Spacebar key) &nbsp; : Jump

<br>
<h1>Project Details</<u></h1>

<h2 id="Env">Environment:</h2>

The Game Environment has each individual [Episode]("Episode") constrained by a limited time duration i.e. <b><em>20 seconds</em></b> within which both [Player]("Player") & [Agent]("Agent") must interact with the Environment & compete to collect as many coins as possible.

Before the game begins, Coins are spawned at random locations within the game environment. The positions of the coins will change with every reset of the environment - environment state reset occurs at the begin of each game session or [episode](#Episode).

The State Vector consists of a Vector of <b><em>N</em></b> features obtained from the agent's velocity, along with <em>ray-based perception</em> of objects around the agent's forward direction. 

<br>
The <b>Action-Space</b> is composed of <b>5</b> Discrete Actions are available to the <a href="#Agent">Agent</a> which compose the . They are as follows:

* 0 : Move Forward
* 1 : Turn Left
* 2 : Turn Right
* 3 : Move Backward
* 4 : Jump

The [Agent](#Agent) can choose among 5 discrete actions at each timestep of an Episode. 

<br>
<h2 id="Player">Player: </h2>

The player can interact with the game [Environment](#E) / [Agent](#Agent) using the <a href="#player-controls">Player Controls</a>

<br>
<h2 id="Agent">Agent: </h2>

The Reinforcement Learning Agent is based on the <b>Proximal Policy Optimization</b> algorithm, which is an advanced policy-based RL algorithm. 

At each timestep in the Episode the Agent takes an action which follows the optimal policy learnt to collect maximum number of coins in an Episode.

<br>
<h2 id="Episode">Episode: </h2>

Each Episode / Game play-time session is restricted to <b><em>20 seconds</em></b> within which the Agent must interact with the [environment](#Env) to collect as many Coins as possible.

<br>

<h1>Game Assets:</h1>

Most of the assets used in this project are available for free from the <a href="https://assetstore.unity.com/">Unity Asset Store</a>.

<b>Player Avatar</b> has been created with the <a href="https://assetstore.unity.com/packages/3d/characters/jammo-character-mix-and-jam-158456#description">Jammo Asset</a> - a friendly bot character 🤖 presented by <b><a href="https://www.youtube.com/watch?v=jKErxSUx54Q">Mix and Jam</a></b>.

<b>Agent Avatar</b> is self-created.

All <b>Avatar</b> animations i.e. running, jumping - have been appropriated from <b><a href="https://www.mixamo.com/">Mixamo</a></b>.

<hr style = "border: 2px gray double"></hr>
