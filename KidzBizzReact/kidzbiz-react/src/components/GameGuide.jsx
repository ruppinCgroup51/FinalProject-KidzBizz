import React from "react";
import { Link, useNavigate } from "react-router-dom";
import "../css/GameGuide.css" // Import your CSS file for styling

export default function GameGuide() {
  const navigate = useNavigate();

  const handleClose = () => {
    navigate("/Lobi"); // Navigate back to the Lobi page
  };

  return (
    <div className="game-guide-overlay"> {/* Overlay container */}
      <div className="game-guide-modal"> {/* Modal container */}
        <h1>מדריך משחק</h1>
        {/*הכנסתי מלל בנתיים לראות איך זה נראה*/}
        <p>
        <br/>
        המשחק "מונופול של קידזביז" הוא משחק שבו לומדים על כסף, כלכלה ופיננסים. במשחק, קונים, משכירים או מוכרים נכסים חשובים, עונים על שאלות בנושא פיננסים, מרוויחים כסף ומתעשרים. מי שהכי עשיר בסוף המשחק - הוא המנצח! 
<br/>
<br/>
במהלך המשחק, אתם יכולים לראות את לוח המשחק, את המיקום של השחקנים על הלוח, את הנכסים שקניתם ואת הכסף שיש לכם כרגע.
<br/>
<br/>
בהתחלת המשחק, כל שחקן יקבל 1,500 שקלים כדי להתחיל איתם. <br/>
כל שחקן מתחיל במקום שנקרא "צא", וכל סיבוב מתקדם על הלוח לפי התוצאה של קוביית המשחק.
<br/>
כששחקן מגיע למשבצת של "נכס", הוא יכול לבחור אם הוא רוצה לקנות את הנכס או לא. <br/> שימו לב! בעלי הנכסים גובים דמי שכירות מהשחקנים האחרים שנעצרים על הנכסים שלהם, לכן כדאי לקנות כמה שיותר נכסים. 
<br/>
כששחקן מגיע למשבצת של "בית כלא", הוא חייב להיכנס לכלא ולהישאר שם תור אחד שלם.<br/>

על לוח המשחק מפוזרים גם קלפי הפתעה וקלפי סיכוי. השחקן חייב תמיד למלא את מה שכתוב על קלפי ההפתעה והמשימה, גם אם זה אומר להישלח לכלא 😣.<br/>
בנוסף, יש גם קלפי "ידע" שמפוזרים על הלוח. קלפים אלו מכילים מידע לימודי על כלכלה ופיננסים. אחרי קריאת הקלף, השחקן יכול לבחור אם הוא רוצה לענות על שאלה בנושא ולזכות בכסף.
<br/>
<br/>
שימו לב! ככל שיש לכם יותר כסף, כך גדלים הסיכויים שלכם לנצח במשחק, אז היו מרוכזים וענו על כמה שיותר שאלות 😀
<br/>
כששחקן פושט רגל, הוא יוצא מהמשחק.<br/>
המטרה במשחק היא להיות השחקן האחרון שלא פשט רגל. <br/>
השחקן הכי עשיר - מנצח במשחק 👏.
     

        </p>
        <button onClick={handleClose}>סגור</button> {/* Close button to navigate back to Lobi */}
      </div>
    </div>
  );
}
