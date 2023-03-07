
import { Person } from './models/Person';
import { Pet } from './models/Pet';
import { PetService } from './services/PetService';

let service = new PetService();

var petToRescue = new Pet(
    "Maricel", 
    "https://i.imgur.com/AO6wMYS.jpeg",
    "Cat",
    "AAAAA",
    new Date(),
    8,
    new Person("Costel", "1234567890123")
)

service.rescue(petToRescue)
    .then(() => 
        service.getAll()
        .then(pets => console.log(pets))
    );
    
console.log("hello");

    import confetti from 'canvas-confetti';

    confetti.create(document.getElementById('canvas') as HTMLCanvasElement,{
        resize: true,
        useWorker: true,
    })({particleCount: 200, spread: 200});