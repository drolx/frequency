/* DEPRECIATED: Old active line hak for Asp.net */
function handleActiveNav(targetClass) {
   document.addEventListener('DOMContentLoaded', () => {
       document.querySelectorAll(targetClass).forEach(link => {
           const activePath = location.pathname.toLowerCase();
           const currentPath = link.getAttribute('href').toLowerCase();
           const root = currentPath.length === 1;

           if (root && currentPath === activePath) link.classList.add('active');
           else if (!root && activePath.includes(currentPath)) link.classList.add('active');
           else {
               link.classList.remove('active');
           }
       });
   })
}

