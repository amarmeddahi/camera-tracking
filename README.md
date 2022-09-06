# camera-tracking

The objective of this repository is to propose a module under Unity3D which allows to realize
a camera trajectory tracking of an object by providing only a limited amount of information,
namely a few points.

<p align="center">
  <img src="camera.gif" />
</p>

# Tutorial

* Create a basic 3D Unity project
* Add the script "trajectory.cs" in the project assets
  * Add to the object "Main Camera" the component (scripts) "trajectory.cs"
* Place an object in the scene: the subject
  * Create a tag "sujet"
  * Assign to this object the tag "sujet"
* Add observers around the object (e.g. 4 balls forming a square around the subject)
  * Create a tag "points"
  * Assign to all observers the tag "points"
* Play
