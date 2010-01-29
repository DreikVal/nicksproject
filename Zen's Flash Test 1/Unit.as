package  {
	import flash.display.MovieClip;
	import flash.events.TimerEvent;
	
	
	public class Unit extends MovieClip {
		
		// Constants:
		// Public Properties:
		// Private Properties:
		private var unitClip:MovieClip;
	
		// Initialization:
		public function Unit() 
		{
			unitClip = new MovieClip();
			unitClip.graphics.beginFill(0xff0000, 1);
			unitClip.graphics.drawRect(unitClip.x, unitClip.y, 20, 20);
			unitClip.graphics.endFill();
			unitClip.x = 40;
			unitClip.y = 40;
			
			addChild(unitClip);
		}
	
		// Public Methods:
		public function step(timeEvent:TimerEvent):void
		{
			unitClip.y++;
		}
		// Protected Methods:
	}
	
}